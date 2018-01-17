using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PopulationStratifiedSampling
{
    public partial class Form1 : Form
    {
        private readonly Random _random = new Random();
        private const int DefaultCellCountX = 20;
        private const int DefaultCellCountY = 20;
        private int _cellCountX = DefaultCellCountX;
        private int _cellCountY = DefaultCellCountY;
        private int _cellCount = DefaultCellCountX * DefaultCellCountY;
        private const int DefaultMinPopulationDensity = 0;
        private const int DefaultLocalPopulationDensity = 100;
        private const int DefaultGlobalPopulationDensity = 25;
        private const double DefaultLocalPopulationDensitySamplingProbability = 0.1;

        public Form1()
        {
            InitializeComponent();
        }

        private int Get1DIndex(Tuple<int, int> index2D)
        {
            return index2D.Item1 * _cellCountY + index2D.Item2;
        }

        private Tuple<int, int> Get2DIndex(int index1D)
        {
            return new Tuple<int, int>(index1D / _cellCountY, index1D % _cellCountY);
        }

        private void GetRandomPopulationDensities(out SortedDictionary<int, int> cellIndexToPopulationDensity,
            out SortedDictionary<int, int> cellIndexToInfectedCounts)
        {
            cellIndexToPopulationDensity = new SortedDictionary<int, int>();
            cellIndexToInfectedCounts = new SortedDictionary<int, int>();

            if (!int.TryParse(cmbMaxGlobalPopulationDensity.Text, out int maxGlobalPopulationDensity))
                maxGlobalPopulationDensity = DefaultGlobalPopulationDensity;
            if (!int.TryParse(cmbMaxLocalPopulationDensity.Text, out int maxLocalPopulationDensity))
                maxLocalPopulationDensity = DefaultLocalPopulationDensity;
            if (!double.TryParse(cmbLocalSamplingProbability.Text, out double maxLocalPopulationDensitySamplingProbability))
                maxLocalPopulationDensitySamplingProbability = DefaultLocalPopulationDensitySamplingProbability;

            // Check ranges
            if (maxGlobalPopulationDensity < 0)
                maxGlobalPopulationDensity = DefaultGlobalPopulationDensity;
            if (maxLocalPopulationDensity < 0)
                maxLocalPopulationDensity = DefaultLocalPopulationDensity;
            if (maxLocalPopulationDensitySamplingProbability < 0 || maxLocalPopulationDensitySamplingProbability > 1)
                maxLocalPopulationDensitySamplingProbability = DefaultLocalPopulationDensitySamplingProbability;

            // Get random population densities for a grid
            for (int index = 0; index < _cellCount; index++)
            {
                int randomPopulationDensity = _random.Next(DefaultMinPopulationDensity, 
                    _random.NextDouble() < maxLocalPopulationDensitySamplingProbability 
                        ? maxLocalPopulationDensity : maxGlobalPopulationDensity);

                cellIndexToPopulationDensity[index] = randomPopulationDensity;
                cellIndexToInfectedCounts[index] = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tpbProgress.Value = tpbProgress.Minimum;
            Stopwatch watch = Stopwatch.StartNew();
            ttxStatus.Text = @"Working...";
            
            if (!int.TryParse(cmbCellCountX.Text, out _cellCountX))
                if (_cellCountX < 0)
                    _cellCountX = DefaultCellCountX;
            if (!int.TryParse(cmbCellCountY.Text, out _cellCountY))
                if (_cellCountY < 0)
                    _cellCountY = DefaultCellCountY;
            _cellCount = _cellCountX * _cellCountY;

            // Get random population densities for a grid
            GetRandomPopulationDensities(out SortedDictionary<int, int> cellIndexToPopulationDensity,
                out SortedDictionary<int, int> cellIndexToInfectedCounts);

            tpbProgress.PerformStep();

            // Make strata
            Dictionary<Tuple<Tuple<int, int>, Tuple<int, int>>, int> strataBoundingBoxesToSampleCount =
                new Dictionary<Tuple<Tuple<int, int>, Tuple<int, int>>, int>
                {
                    [new Tuple<Tuple<int, int>, Tuple<int, int>>(
                        new Tuple<int, int>(0, 0), new Tuple<int, int>(4, 4))] = 25,
                    [new Tuple<Tuple<int, int>, Tuple<int, int>>(
                        new Tuple<int, int>(0, 8), new Tuple<int, int>(9, 9))] = 25,
                    [new Tuple<Tuple<int, int>, Tuple<int, int>>(
                        new Tuple<int, int>(15, 5), new Tuple<int, int>(17, 17))] = 25
                };
            
            // Clear & populate datagridview of strata
            dgvStrata.Rows.Clear();
            foreach (KeyValuePair<Tuple<Tuple<int, int>, Tuple<int, int>>, int> keyValuePair in strataBoundingBoxesToSampleCount)
            {
                dgvStrata.Rows.Add(keyValuePair.Key.Item1.Item1, keyValuePair.Key.Item1.Item2, keyValuePair.Key.Item2.Item1, 
                    keyValuePair.Key.Item2.Item2, keyValuePair.Value);
            }

            tpbProgress.PerformStep();
            
            foreach (KeyValuePair<Tuple<Tuple<int, int>, Tuple<int, int>>, int> currentStratum in strataBoundingBoxesToSampleCount)
            {
                var currentCellIndexToPopulationDensity = StratumCellIndexToPopulationDensity(cellIndexToPopulationDensity, currentStratum);

                SampleStratifiedPopulation(cellIndexToInfectedCounts,
                    currentStratum.Key,
                    currentCellIndexToPopulationDensity,
                    cellIndexToPopulationDensity,
                    currentStratum.Value);
            }

            tpbProgress.PerformStep();

            PrintShowResults(cellIndexToInfectedCounts, _cellCountX, _cellCountY, cellIndexToPopulationDensity,
                cellIndexToPopulationDensity.Values.Max(), strataBoundingBoxesToSampleCount);
            
            watch.Stop();
            tlbExecutionTime.Text = watch.ElapsedMilliseconds.ToString();
            ttxStatus.Text = @"Done.";
            tpbProgress.Value = tpbProgress.Maximum;
        }

        private SortedDictionary<int, int> StratumCellIndexToPopulationDensity(SortedDictionary<int,
            int> cellIndexToPopulationDensity, KeyValuePair<Tuple<Tuple<int, int>, Tuple<int, int>>, int> currentStratum)
        {
            SortedDictionary<int, int> currentCellIndexToPopulationDensity =
                new SortedDictionary<int, int>();

            int minX = currentStratum.Key.Item1.Item1;
            int minY = currentStratum.Key.Item1.Item2;
            int maxX = currentStratum.Key.Item2.Item1;
            int maxY = currentStratum.Key.Item2.Item2;
            for (int x = minX; x < maxX; x++)
            {
                for (int y = minY; y < maxY; y++)
                {
                    Tuple<int, int> cellIndex = new Tuple<int, int>(x, y);
                    int currentPopulationDensity = cellIndexToPopulationDensity[Get1DIndex(cellIndex)];
                    currentCellIndexToPopulationDensity[Get1DIndex(cellIndex)] = currentPopulationDensity;
                }
            }
            return currentCellIndexToPopulationDensity;
        }

        private void SampleStratifiedPopulation(
            SortedDictionary<int, int> cellIndexToInfectedCounts,
            Tuple<Tuple<int, int>, Tuple<int, int>> strataBbox,
            SortedDictionary<int, int> currentCellIndexToPopulationDensity,
            SortedDictionary<int, int> cellIndexToPopulationDensity, int sampleSize)
        {
            // Progressively accumulate the total population densities in a series -> grid index
            Dictionary<int, int> cumulativePopulationDensitiesToCellIndex =
                CumulativePopulationDensitiesToCellIndex(currentCellIndexToPopulationDensity);

            // Iterate all Natural numbers (0, max cumulative density)
            int populationDensityValueMax = 0;
            List<int> populationDensityValueToCellIndex = new List<int>();

            if (cumulativePopulationDensitiesToCellIndex.Count > 0)
            {
                populationDensityValueMax = cumulativePopulationDensitiesToCellIndex.Keys.Max();
                
                int rangeMin = 0;
                int currentCellIndex = cumulativePopulationDensitiesToCellIndex.First().Value;
                for (int i = 0; i < populationDensityValueMax; i++)
                    populationDensityValueToCellIndex.Add(currentCellIndex);
                foreach (KeyValuePair<int, int> keyValuePair in cumulativePopulationDensitiesToCellIndex)
                {
                    int rangeMax = keyValuePair.Key;
                    for (int i = rangeMin; i < rangeMax; i++)
                        populationDensityValueToCellIndex[i] = keyValuePair.Value;
                    rangeMin = rangeMax;
                }
            }

            // Sample within the distribution (0, maxCumulativePopulationDensity)
            if (populationDensityValueMax > 0)
            {
                for (int i = 0; i < sampleSize;)
                {
                    int randomInt = _random.Next(0, populationDensityValueMax);

                    if (populationDensityValueToCellIndex.Count > randomInt)
                    {
                        int cellIndex = populationDensityValueToCellIndex[randomInt];

                        if (cellIndexToInfectedCounts.ContainsKey(cellIndex))
                            cellIndexToInfectedCounts[cellIndex]++;
                        else
                            cellIndexToInfectedCounts[cellIndex] = 1;
                        i++;
                    }
                }
            }
            else
            {
                for (int i = 0; i < sampleSize; i++)
                {
                    int cellIndex = Get1DIndex(new Tuple<int, int>(
                        _random.Next(0, Get2DIndex(currentCellIndexToPopulationDensity.Last().Key).Item1),
                        _random.Next(0, Get2DIndex(currentCellIndexToPopulationDensity.Last().Key).Item2)));

                    if (cellIndexToInfectedCounts.ContainsKey(cellIndex))
                        cellIndexToInfectedCounts[cellIndex]++;
                    else
                        cellIndexToInfectedCounts[cellIndex] = 1;
                }
            }

            // Print results
            int minX = strataBbox.Item1.Item1;
            int minY = strataBbox.Item1.Item2;
            int maxX = strataBbox.Item2.Item1;
            int maxY = strataBbox.Item2.Item2;
            if (chkShowTextLog.Checked)
            {
                txtSamples.Clear();
                int sumSamples = 0;
                for (int index = 0; index < _cellCount; index++)
                {
                    int x = Get2DIndex(index).Item1;
                    int y = Get2DIndex(index).Item2;
                    int currentSamples = cellIndexToInfectedCounts[index];
                    if (currentSamples > 0 || (x >= minX && x < maxX && y >= minY && y < maxY ))
                    {
                        sumSamples += currentSamples;
                        txtSamples.AppendText($"[{x}, " +
                                                $"{y}]: {currentSamples} samples" +
                                                $" (Population Density: {cellIndexToPopulationDensity[index]})\r\n");
                    }
                    
                }

                txtSamples.AppendText($"\r\nTotal Samples: {sumSamples}");
            }
       }

        private void PrintShowResults(SortedDictionary<int, int> cellIndexToInfectedCounts, int cellCountX, int cellCountY,
            SortedDictionary<int, int> cellIndexToPopulationDensity, int maxPopulationDensity,
            Dictionary<Tuple<Tuple<int, int>, Tuple<int, int>>, int> strataBoundingBoxesToSampleCount)
        {
            // Write results to png
            try
            {
                if (pictureBox1.Image != null)
                {
                    pictureBox1.Image.Dispose();
                    pictureBox1.Image = null;
                    pictureBox1.Dispose();
                    pictureBox1 = new PictureBox();

                    ((System.ComponentModel.ISupportInitialize) pictureBox1).BeginInit();
                    pictureBox1.Location = new Point(285, 41);
                    pictureBox1.Name = "pictureBox1";
                    pictureBox1.Size = new Size(400, 400);
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox1.TabIndex = 2;
                    pictureBox1.TabStop = false;
                    Controls.Add(pictureBox1);
                    ((System.ComponentModel.ISupportInitialize) (pictureBox1)).EndInit();
                }

                if (pictureBox2.Image != null)
                {
                    pictureBox2.Image.Dispose();
                    pictureBox2.Image = null;
                    pictureBox2.Dispose();
                    pictureBox2 = new PictureBox();
                    ((System.ComponentModel.ISupportInitialize) (pictureBox2)).BeginInit();
                    pictureBox2.Location = new Point(691, 41);
                    pictureBox2.Name = "pictureBox2";
                    pictureBox2.Size = new Size(400, 400);
                    pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox2.TabIndex = 3;
                    pictureBox2.TabStop = false;
                    Controls.Add(pictureBox2);
                    ((System.ComponentModel.ISupportInitialize) (pictureBox2)).EndInit();
                }
                
                if (File.Exists(@"populationDensity.png"))
                    File.Delete(@"populationDensity.png");
                if (File.Exists(@"infectedSamples.png"))
                    File.Delete(@"infectedSamples.png");
                
                // Get strata bboxes
                List<Tuple<Tuple<int, int>, Tuple<int, int>>> strataBoundingBoxes = new List<Tuple<Tuple<int, int>, Tuple<int, int>>>();
                foreach (KeyValuePair<Tuple<Tuple<int, int>, Tuple<int, int>>, int> keyValuePair in strataBoundingBoxesToSampleCount)
                    strataBoundingBoxes.Add(keyValuePair.Key);

                using (Bitmap b = new Bitmap(cellCountX, cellCountY))
                {
                    using (Graphics g = Graphics.FromImage(b))
                    {
                        g.Clear(Color.White);
                        for (int index = 0; index < _cellCount; index++)
                        {
                            int x = Get2DIndex(index).Item1;
                            int y = Get2DIndex(index).Item2;
                            
                            int colorIntensity = 255 - (int) (255 * (double) cellIndexToPopulationDensity[index] / maxPopulationDensity);
                            Color c = Color.FromArgb(colorIntensity, colorIntensity, 255);

                            if (cellIndexToPopulationDensity[index] > 0)
                                b.SetPixel(x, y, c);

                            // Draw black outlines of strata, on infected
                            foreach (Tuple<Tuple<int, int>, Tuple<int, int>> strataBoundingBox in strataBoundingBoxes)
                            {
                                int minX = strataBoundingBox.Item1.Item1 - 1;
                                int maxX = strataBoundingBox.Item2.Item1 + 1;
                                int minY = strataBoundingBox.Item1.Item2 - 1;
                                int maxY = strataBoundingBox.Item2.Item2 + 1;

                                if (x == minX || x == maxX)
                                    if (y >= minY && y <= maxY)
                                        b.SetPixel(x, y, Color.Black); // Black

                                if (y == minY || y == maxY)
                                    if (x >= minX && x <= maxX)
                                        b.SetPixel(x, y, Color.Black); // Black
                            }
                            
                        }
                        g.Dispose();
                    }

                    b.Save(@"populationDensity.png", ImageFormat.Png);
                    b.Dispose();
                }
                
                int maxInfectedCounts = cellIndexToInfectedCounts.Values.Max();
                using (Bitmap b = new Bitmap(cellCountX, cellCountY))
                {
                    using (Graphics g = Graphics.FromImage(b))
                    {
                        g.Clear(Color.White);
                        for (int index = 0; index < _cellCount; index++)
                        {
                            int x = Get2DIndex(index).Item1;
                            int y = Get2DIndex(index).Item2;
                            int colorIntensity = 255 - (int) (255 * cellIndexToInfectedCounts[index]
                                                                / (double) maxInfectedCounts);
                            Color c = Color.FromArgb(255, colorIntensity, colorIntensity);

                            if (cellIndexToInfectedCounts[index] > 0)
                                b.SetPixel(x, y, c);
                                
                            // Draw black outlines of strata, on infected
                            foreach (Tuple<Tuple<int, int>, Tuple<int, int>> strataBoundingBox in strataBoundingBoxes)
                            {
                                int minX = strataBoundingBox.Item1.Item1 - 1;
                                int maxX = strataBoundingBox.Item2.Item1 + 1;
                                int minY = strataBoundingBox.Item1.Item2 - 1;
                                int maxY = strataBoundingBox.Item2.Item2 + 1;
                                    
                                if (x == minX || x == maxX)
                                    if (y >= minY && y <= maxY)
                                        b.SetPixel(x, y, Color.Black); // Black

                                if (y == minY || y == maxY)
                                    if (x >= minX && x <= maxX)
                                        b.SetPixel(x, y, Color.Black); // Black
                            }
                            
                        }
                        g.Dispose();
                    }

                    b.Save(@"infectedSamples.png", ImageFormat.Png);
                    b.Dispose();
                }
                
                // Load png: population densities + samples
                pictureBox1.Load(@"populationDensity.png");
                pictureBox2.Load(@"infectedSamples.png");
                
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private static Dictionary<int, int> CumulativePopulationDensitiesToCellIndex(
            SortedDictionary<int, int> cellIndexToPopulationDensity)
        {
            Dictionary<int, int> cumulativePopulationDensitiesToCellIndex = new Dictionary<int, int>();
            int populationDensitiesSum = 0;
            foreach (KeyValuePair<int, int> keyValuePair in cellIndexToPopulationDensity)
            {
                int densityValue = keyValuePair.Value;
                if (densityValue > 0)
                {
                    populationDensitiesSum += densityValue;
                    if (!cumulativePopulationDensitiesToCellIndex.ContainsKey(populationDensitiesSum))
                        cumulativePopulationDensitiesToCellIndex[populationDensitiesSum] = keyValuePair.Key;
                }
            }

            return cumulativePopulationDensitiesToCellIndex;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void dgvStrata_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
