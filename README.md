# Crawler


## Usage

`>crawler.exe [crawl name] [contact info] [crawl seeds]`

- Crawl name: Any filepath-safe name
- Contact info: Included in Crawler user agent
- Crawl seeds: Initial sites to visit

### Other Inputs

The file `./comparisonText.txt` is used to give each page crawled a score of compatibility with the included terms.

Scored pages are written to disk after each page crawl to `[crawl name]_candidates.txt`