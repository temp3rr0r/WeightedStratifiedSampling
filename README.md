Weighted stratified sampling

Performs stratified random sampling on specific regions of a grid. Sampling is weighted by an additional layer (i.e population density).
In our case, the sample values represent spatial-based infected counts (infected hosts), for specific regions (i.e an administrative level) of a grid.

The population density layer random generates values within a global min (== 0) and max. In some locations, it samples from a local min/max, to represent
dense cities. So, for a specific cell range of a range, the population densities are progressively being accumulated in a series of natural numbers - to - cell index. 
Randomly sampling from that series, can approach the real population densities.

Some grid/cell/sampling values are being configurable. Results are being represented in image format. Colors are scaled to min/max values of blue for population densities
and of red for infected host samples. Strata are being outlined with black.