import { Component, OnInit } from '@angular/core';
import { NgbDateStruct, NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { ActivatedRoute, Router } from '@angular/router';
import { ShipTimesheetApiService } from '../services/ship-timesheet-api.service';

@Component({
  selector: 'app-ships-edit',
  templateUrl: './ships-edit.component.html',
  styleUrls: ['./ships-edit.component.css']
})
export class ShipsEditComponent implements OnInit {

  public ship: any = {};
  public events = [];
  public skippers: any[];
  public loading = false;
  public startDate: any;
  public maxDate: NgbDateStruct;

  sortKey: string;
  sortOptions: SelectItem[];

  constructor(private router: ActivatedRoute,
              private nav: Router,
              private api: ShipTimesheetApiService) {
      let today = new Date();
      this.maxDate = NgbDate.from({ year: today.getFullYear(), month: today.getMonth() + 1, day: today.getDate() + 1 });
     }

  ngOnInit() {
    this.api.getSkippers().subscribe(data => {
      this.skippers = data;
    });
    this.router.params.subscribe(params => {
      if (params.id !== 'new') {
        this.loading = true;
        this.api.getShip(params.id).subscribe(data => {
          this.ship = data;
          this.events = data.events;
          this.sortOptions = [
            {label: 'Newest First', value: '!1eventTime'},
            {label: 'Oldest First', value: 'eventTime'}
        ];
          this.loading = false;
        });
      }
    });
  }

  save() {
    this.loading = true;
    this.api.saveShip(this.ship).subscribe(data => {
      this.loading = false;
      this.nav.navigate(['/ships']);
    });
  }

  popVisibilityChanged(pop) {
    console.log(`Popover open state: ${pop.isOpen()}`);
  }

  onSortChange() {
    if (this.sortKey.indexOf('!') === 0)
        this.sort(-1);
    else
        this.sort(1);
}

sort(order: number): void {
    let events = [...this.events];
    events.sort((data1, data2) => {
        let value1 = data1.eventTime;
        let value2 = data2.eventTime;
        let result = (value1 < value2) ? -1 : (value1 > value2) ? 1 : 0;

        return (order * result);
    });

    this.events = events;
}

}

export interface SelectItem {
  label?: string;
  value: any;
  styleClass?: string;
  icon?: string;
  title?: string;
  disabled?: boolean;
}
