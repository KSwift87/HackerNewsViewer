import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Story } from '../shared/models/story.model';
import { firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class StoriesService {

  private _url: string = 'https://localhost:7285/stories/';

  constructor(private _http: HttpClient) { }

  async getStories(type: string, page: number = 1, storiesPerPage: number = 30) {
    const url = `${this._url}GetStoriesAsync?type=${type}&page=${page}&storiesPerPage=${storiesPerPage}`;
    
    return await firstValueFrom(this._http.get<Story[]>(url));
  }
}
