import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbDateStruct, NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, tap, switchMap } from 'rxjs/operators';
import * as _ from 'lodash';
import { ShipTimesheetApiService } from '../services/ship-timesheet-api.service';

@Component({
  selector: 'app-events-edit',
  templateUrl: './events-edit.component.html',
  styleUrls: ['./events-edit.component.css'],
  encapsulation: ViewEncapsulation.None
})

export class EventsEditComponent implements OnInit {
  public event: any = {};
  public ships: any[];
  public loading = false;
  public startDate: any;
  public maxDate: NgbDateStruct;
  public timeOfEvent: { hour: number; minute: number; } = { hour: 0, minute: 0};
  public dateTime: any;

  constructor(private router: ActivatedRoute,
              private nav: Router,
              private api: ShipTimesheetApiService) {
      let today = new Date();
      this.maxDate = NgbDate.from({ year: today.getFullYear(), month: today.getMonth() + 1, day: today.getDate() + 1 });
     }

  ngOnInit() {
    this.api.getShips().subscribe(data => {
      this.ships = data;
    });
    this.router.params.subscribe(params => {
      if (params.id !== 'new') {
        this.loading = true;
        this.api.getEvent(params.id).subscribe(data => {
          this.event = data;
          this.dateTime = new Date(this.event.eventTime);
          this.startDate = { year: this.dateTime.getFullYear(), month: this.dateTime.getMonth() + 1, day: this.dateTime.getDate() };
          this.timeOfEvent.hour = new Date(this.event.eventTime).getHours();
          this.timeOfEvent.minute = new Date(this.event.eventTime).getMinutes();
          this.loading = false;
        });
      }
    });
  }

  save() {
    this.loading = true;
    // let dateTime = new Date(this.event.eventTime);
    // dateTime.setHours(this.timeOfEvent.hour, this.timeOfEvent.minute);
    let date = new Date(this.startDate.year, this.startDate.month - 1, this.startDate.day);
    date.setHours(this.timeOfEvent.hour);
    date.setMinutes(this.timeOfEvent.minute);
    this.event.eventTime = date.toISOString();
    this.api.saveEvent(this.event).subscribe(data => {
      this.loading = false;
      this.nav.navigate(['/events']);
    });
  }

  popVisibilityChanged(pop) {
    console.log(`Popover open state: ${pop.isOpen()}`);
  }
}
