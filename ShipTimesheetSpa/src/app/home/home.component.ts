import { Component, OnInit } from '@angular/core';
import { ShipTimesheetApiService } from '../services/ship-timesheet-api.service';
import {DayPilot, DayPilotSchedulerComponent, DayPilotCalendarComponent} from 'daypilot-pro-angular';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  config: any = {
    viewType: 'Day',
    startDate: '2020-04-13',
    days: 29
  };
  public events = [];
  public eventsTo = [];

  constructor(private api: ShipTimesheetApiService, private datePipie: DatePipe) { }

  ngOnInit() {
    this.api.getEvents().subscribe(data => {
      this.events = data;
      this.MapData();
  });
  }

  MapData() {
    this.events.forEach(element => {
      let eventTo = new EventTo();
      eventTo.id = element.eventId;
      eventTo.resource = element.ship.name;
      eventTo.start = this.datePipie.transform(element.eventTime, 'yyyy-MM-dd');
      eventTo.end = this.datePipie.transform(element.eventTime, 'yyyy-MM-dd');
      eventTo.text = element.ship.name;
      this.eventsTo.push(eventTo);
    });
  }
}

export class EventTo
{
  id: any;
  start: any;
  end: any;
  text: string;
  resource: string;
}
