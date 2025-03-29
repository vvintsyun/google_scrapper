import { Component } from '@angular/core';
import { ScrapperService } from '../services/scrapper.service';
import { SearchRequest } from '../models/search.request';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  keyword: string = '';
  url: string = '';
  result: string = '';
  isLoading: boolean = false;
  error: string = '';

  constructor(private service: ScrapperService) {}

  search() {
    if (!this.keyword.trim() || !this.url.trim()) {
      this.error = 'Both fields are required.';
      return;
    }

    this.isLoading = true;
    this.error = '';
    this.result = '';

    const input: SearchRequest = {
      keyword: this.keyword,
      url: this.url
    };
    this.service.getData(input).subscribe(
      (data) => {
        this.result = `Found at positions: ${data.searchResults}`;
        this.error = '';
        this.isLoading = false;
      },
      (err) => {
        this.error = 'Failed to fetch results. Try again later.';
        this.result = '';
        this.isLoading = false;
      }
    );
  }
}
