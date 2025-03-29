# google_scrapper

To run application setup startup projects:
1. Right click on Solution
2. Configure startup projects
3. Select multiple startup projects and select start for both scrapper.client and scrapper.server

Manual scrapping without API or 3rd party libraries is not feasible due to Google protection (see results of executing normal flow in \Tests\TestData\test.html)
There were attempts to use cookies from the active browser session that also did not help. To try this approach use code in lines 44-89 ScrapperService.cs

Hence, the only way to test parser algorithm is 
1. Manually go to https://www.google.co.uk/search?num=100&q=myrequest with desired parameters
2. Copy content of website from DevTools - Sources
3. Save as .html file in your local directory
4. Run within test from Tests project (see other tests for examples)
