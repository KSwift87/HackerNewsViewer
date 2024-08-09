import { Component,  OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { Subscription } from 'rxjs';
import { Story } from '../shared/models/story.model';
import { StoriesService } from './stories.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-stories',
  standalone: true,
  imports: [CommonModule],
  providers: [StoriesService],
  templateUrl: './stories.component.html',
  styleUrl: './stories.component.scss'
})
export class StoriesComponent implements OnInit, OnDestroy {

  subscriptions: Subscription[] = new Array<Subscription>();

  error: string = '';
  
  storyText: string = '';
  storyType: string = '';
  page: number = 1;
  storiesPerPage: number = 15;
  stories: Story[] = new Array<Story>();

  constructor(private _activatedRoute: ActivatedRoute, 
    private _storiesService: StoriesService) {}

  ngOnInit(): void {
    this.subscriptions.push(this._activatedRoute.params.subscribe(async p => {
      await this.loadData(p);
    }));
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub?.unsubscribe());
  }

  async loadData(p: Params) {
    this.page = 1
    this.storyText = '';

      if (p['type'] != this.storyType) {
        this.stories = new Array<Story>();
        this.storyType = p['type'];
      }

      // Normally I'd handle errors via a global handler or something like that, but this is a PoC sample app.
      try {
        this.stories = await this._storiesService.getStories(this.storyType, this.page, this.storiesPerPage);
        this.error = '';
      } catch (error)
      {
        console.log(error);
        this.error = 'Failed to load stories!';
      }
  }

  rowClick(id: number): void {
    const story = this.stories.find(s => s.id === id);

    if (story?.text) {
      this.storyText = story.text;
    } else if (story?.url) {
      window.location.href = story.url;
    }
  }

  async nextClick($event: any): Promise<void> {
    $event.preventDefault();
    this.page++;
    this.stories = await this._storiesService.getStories(this.storyType, this.page, this.storiesPerPage);
  }

  async prevClick($event: any): Promise<void> {
    $event.preventDefault();
    this.page--;
    this.stories = await this._storiesService.getStories(this.storyType, this.page, this.storiesPerPage);
  }

  storyTextClick() {
    this.storyText = '';
  }
}
