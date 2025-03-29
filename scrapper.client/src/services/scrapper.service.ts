import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { SearchRequest } from '../models/search.request';

@Injectable()
export class ScrapperService  {
  constructor(private http: HttpClient) {    
  }

  getData(input: SearchRequest): Observable<any> {
    const params = new HttpParams()
      .set('keyword', input.keyword)
      .set('url', input.url);

    return this.http.get('/api/scrapper', { params });
  }
}
