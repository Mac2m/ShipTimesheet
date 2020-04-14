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
  public timeOfEvent: any;

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
          let d = new Date(this.event.eventTime);
          this.startDate = { year: d.getFullYear(), month: d.getMonth() + 1 };
          this.loading = false;
        });
      }
    });
  }

  save() {
    this.loading = true;
    let dateTime = new Date(this.event.eventTime);
    dateTime.setHours(this.timeOfEvent.hour, this.timeOfEvent.minute);
    this.event.eventTime = dateTime;
    this.api.saveEvent(this.event).subscribe(data => {
      this.loading = false;
      this.nav.navigate(['/events']);
    });
  }

  popVisibilityChanged(pop) {
    console.log(`Popover open state: ${pop.isOpen()}`);
  }
}
