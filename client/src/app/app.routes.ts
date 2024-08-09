import { Routes } from '@angular/router';
import { StoriesComponent } from './stories/stories.component';
import { NotFoundComponent } from './not-found/not-found.component';

export const routes: Routes = [
    { path: 'stories/:type', component: StoriesComponent },
    { path: 'stories', component: StoriesComponent },
    { path: '', redirectTo: 'stories', pathMatch: 'full' },
    { path: "**", component: NotFoundComponent}
];
