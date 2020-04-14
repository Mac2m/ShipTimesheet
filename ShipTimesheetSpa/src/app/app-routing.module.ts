import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ShipsComponent } from './ships/ships.component';
import { SkippersComponent } from './skippers/skippers.component';
import { HomeComponent } from './home/home.component';
import { ShipsEditComponent } from './ships-edit/ships-edit.component';
import { SkippersEditComponent } from './skippers-edit/skippers-edit.component';
import { EventsComponent } from './events/events.component';
import { EventsEditComponent } from './events-edit/events-edit.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'ships', component: ShipsComponent },
  { path: 'ships/:id', component: ShipsEditComponent},
  { path: 'skippers', component: SkippersComponent },
  { path: 'skippers/:id', component: SkippersEditComponent },
  { path: 'events', component: EventsComponent },
  { path: 'events/:id', component: EventsEditComponent },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
