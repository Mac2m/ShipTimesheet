import { Component, OnInit } from '@angular/core';
import { ShipTimesheetApiService } from '../services/ship-timesheet-api.service';
import * as _ from 'lodash';
import { NgbDateStruct, NgbDate } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-events',
  templateUrl: './events.component.html',
  styleUrls: ['./events.component.css']
})
export class EventsComponent implements OnInit {

  public events = [];
  public loading = false;
  public totals = {};
  public pageSize = 5;
  public currPage = 1;
  public dateFilter: {year: any, month: any, day: any};
  public dateNow = new Date();
  public filteredEvents = [];
  public maxDate: NgbDateStruct;

constructor(private api: ShipTimesheetApiService) {
  this.maxDate = NgbDate.from({ year: this.dateNow.getFullYear(),
     month: this.dateNow.getMonth() + 1,
     day: this.dateNow.getDate() + 1 });
 }

ngOnInit() {
    this.fetchData();
  }

fetchData() {
  this.loading = true;
  this.api.getEvents().subscribe(data => {
    this.events = data;
    this.filteredEvents = this.events;
    this.loading = false;
  });
}

deleteEvent(id) {
    this.api.deleteEvent(id).subscribe(data => this.fetchData());
  }

filterData() {
  this.loading = true;
  this.filteredEvents = this.events.filter(item => {
      return new Date(item.eventTime).toDateString() === new Date(this.dateFilter.year, this.dateFilter.month - 1, this.dateFilter.day).toDateString();
    }
  );
  this.loading = false;
  }

clear() {
  this.dateFilter = undefined;
  this.filteredEvents = this.events;
}

}
