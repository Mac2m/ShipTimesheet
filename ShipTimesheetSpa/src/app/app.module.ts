import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import { DatePipe } from '@angular/common';

// Third party modules
import { NgxLoadingModule } from 'ngx-loading';
import { NgbModule, NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';
import {DayPilotModule} from "daypilot-pro-angular";

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { ShipsComponent } from './ships/ships.component';
import { SkippersComponent } from './skippers/skippers.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { ShipsEditComponent } from './ships-edit/ships-edit.component';
import { SkippersEditComponent } from './skippers-edit/skippers-edit.component';
import { EventsComponent } from './events/events.component';
import { EventsEditComponent } from './events-edit/events-edit.component';
import { HttpClientModule } from '@angular/common/http';
import { ShipTimesheetApiService } from './services/ship-timesheet-api.service';
import { FormsModule } from '@angular/forms';
import { DateStringAdapterService } from './services/DateStringAdapter.service';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    ShipsComponent,
    SkippersComponent,
    NavMenuComponent,
    ShipsEditComponent,
    SkippersEditComponent,
    EventsComponent,
    EventsEditComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    NgxLoadingModule.forRoot({}),
    FormsModule,
    NgbModule,
    BrowserAnimationsModule,
    DayPilotModule
  ],
  providers: [
    ShipTimesheetApiService,
    DatePipe,
    { provide: NgbDateAdapter, useClass: DateStringAdapterService }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
