import { Component, OnInit } from '@angular/core';
import { ShipTimesheetApiService } from '../services/ship-timesheet-api.service';
import * as _ from 'lodash';
import { NgbDateStruct, NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { DatePipe } from '@angular/common';

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
  public dateFilter = new Date();
  public filteredEvents = [];
  public maxDate: NgbDateStruct;

constructor(private api: ShipTimesheetApiService, private datePipie: DatePipe) {
  this.maxDate = NgbDate.from({ year: this.dateFilter.getFullYear(),
     month: this.dateFilter.getMonth() + 1,
     day: this.dateFilter.getDate() + 1 });
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
      let itemDate = new Date(item.eventTime);
      let dateTransformed = this.datePipie.transform(itemDate, 'yyyy-MM-dd');
      return dateTransformed === this.datePipie.transform(this.dateFilter, 'yyyy-MM-dd');
    }
  );
  this.loading = false;
  }

clear() {
  this.dateFilter = undefined;
  this.filteredEvents = this.events;
}

}
