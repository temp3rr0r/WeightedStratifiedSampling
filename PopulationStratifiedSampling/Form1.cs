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
        private const int DefaultMinPopulationDensity = 0;
        private const int DefaultLocalPopulationDensity = 100;
        private const int DefaultGlobalPopulationDensity = 25;
        private const double DefaultLocalPopulationDensitySamplingProbability = 0.1;

        public Form1()
        {
            InitializeComponent();
        }

        private void GetRandomPopulationDensities(int cellCountX, int cellCountY,
            out SortedDictionary<Tuple<int, int>, double> cellIndexToPopulationDensity,
            out SortedDictionary<Tuple<int, int>, int> cellIndexToInfectedCounts)
        {
            cellIndexToPopulationDensity = new SortedDictionary<Tuple<int, int>, double>();
            cellIndexToInfectedCounts = new SortedDictionary<Tuple<int, int>, int>();

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
            for (int x = 0; x < cellCountX; x++)
            {
                for (int y = 0; y < cellCountY; y++)
                {
                    Tuple<int, int> cellIndex = new Tuple<int, int>(x, y);
                    
                    int randomPopulationDensity = _random.Next(DefaultMinPopulationDensity, 
                        _random.NextDouble() < maxLocalPopulationDensitySamplingProbability 
                            ? maxLocalPopulationDensity : maxGlobalPopulationDensity);

                    cellIndexToPopulationDensity[cellIndex] = randomPopulationDensity;
                    cellIndexToInfectedCounts[cellIndex] = 0;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // TODO: progress
            tpbProgress.Value = tpbProgress.Minimum;
            Stopwatch watch = Stopwatch.StartNew();
            ttxStatus.Text = @"Working...";

            if (!int.TryParse(cmbCellCountX.Text, out int cellCountX))
                cellCountX = DefaultCellCountX;
            if (!int.TryParse(cmbCellCountY.Text, out int cellCountY))
                cellCountY = DefaultCellCountY;

            // Get random population densities for a grid
            GetRandomPopulationDensities(cellCountX, cellCountY, 
                out SortedDictionary<Tuple<int, int>, double> cellIndexToPopulationDensity,
                out SortedDictionary<Tuple<int, int>, int> cellIndexToInfectedCounts);

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
                dgvStrata.Rows.Add(new Object[]
                {
                    keyValuePair.Key.Item1.Item1, keyValuePair.Key.Item1.Item2, keyValuePair.Key.Item2.Item1,
                    keyValuePair.Key.Item2.Item2, keyValuePair.Value
                });
            }

            tpbProgress.PerformStep();

            // TODO: use 1D for 2D array addressing

            foreach (KeyValuePair<Tuple<Tuple<int, int>, Tuple<int, int>>, int> currentStratum in
                strataBoundingBoxesToSampleCount)
            {
                var currentCellIndexToPopulationDensity = StratumCellIndexToPopulationDensity(cellIndexToPopulationDensity, currentStratum);

                SampleStratifiedPopulation(cellIndexToInfectedCounts,
                    currentStratum.Key,
                    cellCountX, cellCountY,
                    currentCellIndexToPopulationDensity,
                    cellIndexToPopulationDensity,
                    currentStratum.Value);
            }

            tpbProgress.PerformStep();

            PrintShowResults(cellIndexToInfectedCounts, cellCountX, cellCountY, cellIndexToPopulationDensity,
                (int)cellIndexToPopulationDensity.Values.Max(), strataBoundingBoxesToSampleCount);
            
            watch.Stop();
            tlbExecutionTime.Text = watch.ElapsedMilliseconds.ToString();
            ttxStatus.Text = @"Done.";
            tpbProgress.Value = tpbProgress.Maximum;
        }

        private static SortedDictionary<Tuple<int, int>, double> StratumCellIndexToPopulationDensity(SortedDictionary<Tuple<int, int>,
            double> cellIndexToPopulationDensity, KeyValuePair<Tuple<Tuple<int, int>, Tuple<int, int>>, int> currentStratum)
        {
            SortedDictionary<Tuple<int, int>, double> currentCellIndexToPopulationDensity =
                new SortedDictionary<Tuple<int, int>, double>();

            int minX = currentStratum.Key.Item1.Item1;
            int minY = currentStratum.Key.Item1.Item2;
            int maxX = currentStratum.Key.Item2.Item1;
            int maxY = currentStratum.Key.Item2.Item2;
            for (int x = minX; x < maxX; x++)
            {
                for (int y = minY; y < maxY; y++)
                {
                    Tuple<int, int> cellIndex = new Tuple<int, int>(x, y);
                    double currentPopulationDensity = cellIndexToPopulationDensity[cellIndex];
                    currentCellIndexToPopulationDensity[cellIndex] = currentPopulationDensity;
                }
            }
            return currentCellIndexToPopulationDensity;
        }

        private void SampleStratifiedPopulation(
            SortedDictionary<Tuple<int, int>, int> cellIndexToInfectedCounts,
            Tuple<Tuple<int, int>, Tuple<int, int>> strataBbox,int cellCountX, int cellCountY,
            SortedDictionary<Tuple<int, int>, double> currentCellIndexToPopulationDensity,
            SortedDictionary<Tuple<int, int>, double> cellIndexToPopulationDensity, int sampleSize)
        {
            // Progressively accumulate the total population densities in a series -> grid index
            Dictionary<int, Tuple<int, int>> cumulativePopulationDensitiesToCellIndex =
                CumulativePopulationDensitiesToCellIndex(currentCellIndexToPopulationDensity);

            // Iterate all Natural numbers (0, max cumulative density)
            int populationDensityValueMax = 0;
            List<Tuple<int, int>> populationDensityValueToCellIndex = new List<Tuple<int, int>>();

            if (cumulativePopulationDensitiesToCellIndex.Count > 0)
            {
                populationDensityValueMax = cumulativePopulationDensitiesToCellIndex.Keys.Max();
                
                int rangeMin = 0;
                Tuple<int, int> currentCellIndex = cumulativePopulationDensitiesToCellIndex.First().Value;
                for (int i = 0; i < populationDensityValueMax; i++)
                    populationDensityValueToCellIndex.Add(currentCellIndex);
                foreach (KeyValuePair<int, Tuple<int, int>> keyValuePair in cumulativePopulationDensitiesToCellIndex)
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
                        Tuple<int, int> cellIndex = populationDensityValueToCellIndex[randomInt];
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
                    int randomX = _random.Next(0, currentCellIndexToPopulationDensity.Last().Key.Item1);
                    int randomY = _random.Next(0, currentCellIndexToPopulationDensity.Last().Key.Item2);
                    Tuple<int, int> cellIndex = new Tuple<int, int>(randomX, randomY);

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
                for (int x = 0; x < cellCountX; x++)
                {
                    for (int y = 0; y < cellCountY; y++)
                    {
                        Tuple<int, int> cellIndex = new Tuple<int, int>(x, y);
                        int currentSamples = cellIndexToInfectedCounts[cellIndex];
                        if (currentSamples > 0 || (x >= minX && x < maxX && y >= minY && y < maxY ))
                        {
                            sumSamples += currentSamples;
                            txtSamples.AppendText($"[{cellIndex.Item1}, " +
                                                  $"{cellIndex.Item2}]: {currentSamples} samples" +
                                                  $" (Population Density: {cellIndexToPopulationDensity[cellIndex]})\r\n");
                        }
                    }
                }

                txtSamples.AppendText($"\r\nTotal Samples: {sumSamples}");
            }
       }

        private void PrintShowResults(SortedDictionary<Tuple<int, int>, int> cellIndexToInfectedCounts, int cellCountX, int cellCountY,
            SortedDictionary<Tuple<int, int>, double> cellIndexToPopulationDensity, int maxPopulationDensity,
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
                        for (int x = 0; x < cellCountX; x++)
                        {
                            for (int y = 0; y < cellCountY; y++)
                            {
                                Tuple<int, int> cellIndex = new Tuple<int, int>(x, y);
                                int colorIntensity = 255 - (int) (255 * (double) cellIndexToPopulationDensity[cellIndex] / (double) maxPopulationDensity);
                                Color c = Color.FromArgb(colorIntensity, colorIntensity, 255);

                                if (cellIndexToPopulationDensity[cellIndex] > 0)
                                    b.SetPixel(x, y, c);

                                // Draw black outlines of strata, on infected
                                foreach (Tuple<Tuple<int, int>, Tuple<int, int>> strataBoundingBox in strataBoundingBoxes)
                                {
                                    int minX = strataBoundingBox.Item1.Item1 - 1;
                                    int maxX = strataBoundingBox.Item2.Item1 + 1;
                                    int minY = strataBoundingBox.Item1.Item2 - 1;
                                    int maxY = strataBoundingBox.Item2.Item2 + 1;

                                    if (cellIndex.Item1 == minX || cellIndex.Item1 == maxX)
                                        if (cellIndex.Item2 >= minY && cellIndex.Item2 <= maxY)
                                            b.SetPixel(x, y, Color.Black); // Black

                                    if (cellIndex.Item2 == minY || cellIndex.Item2 == maxY)
                                        if (cellIndex.Item1 >= minX && cellIndex.Item1 <= maxX)
                                            b.SetPixel(x, y, Color.Black); // Black
                                }
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
                        for (int x = 0; x < cellCountX; x++)
                        {
                            for (int y = 0; y < cellCountY; y++)
                            {
                                Tuple<int, int> cellIndex = new Tuple<int, int>(x, y);
                                int colorIntensity = 255 - (int) (255 * cellIndexToInfectedCounts[cellIndex]
                                                                  / (double) maxInfectedCounts);
                                Color c = Color.FromArgb(255, colorIntensity, colorIntensity);

                                if (cellIndexToInfectedCounts[cellIndex] > 0)
                                    b.SetPixel(x, y, c);
                                
                                // Draw black outlines of strata, on infected
                                foreach (Tuple<Tuple<int, int>, Tuple<int, int>> strataBoundingBox in strataBoundingBoxes)
                                {
                                    int minX = strataBoundingBox.Item1.Item1 - 1;
                                    int maxX = strataBoundingBox.Item2.Item1 + 1;
                                    int minY = strataBoundingBox.Item1.Item2 - 1;
                                    int maxY = strataBoundingBox.Item2.Item2 + 1;
                                    
                                    if (cellIndex.Item1 == minX || cellIndex.Item1 == maxX)
                                        if (cellIndex.Item2 >= minY && cellIndex.Item2 <= maxY)
                                            b.SetPixel(x, y, Color.Black); // Black

                                    if (cellIndex.Item2 == minY || cellIndex.Item2 == maxY)
                                        if (cellIndex.Item1 >= minX && cellIndex.Item1 <= maxX)
                                            b.SetPixel(x, y, Color.Black); // Black
                                }
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

        private static Dictionary<int, Tuple<int, int>> CumulativePopulationDensitiesToCellIndex(
            SortedDictionary<Tuple<int, int>, double> cellIndexToPopulationDensity)
        {
            Dictionary<int, Tuple<int, int>> cumulativePopulationDensitiesToCellIndex = new Dictionary<int, Tuple<int, int>>();
            int populationDensitiesSum = 0;
            foreach (KeyValuePair<Tuple<int, int>, double> keyValuePair in cellIndexToPopulationDensity)
            {
                int densityValue = (int) keyValuePair.Value;
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
