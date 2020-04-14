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
  public events: any[];
  public skippers: any[];
  public loading = false;
  public startDate: any;
  public maxDate: NgbDateStruct;

  constructor(private router: ActivatedRoute,
              private nav: Router,
              private api: ShipTimesheetApiService) {
      let today = new Date();
      this.maxDate = NgbDate.from({ year: today.getFullYear(), month: today.getMonth() + 1, day: today.getDate() + 1 });
     }

  ngOnInit() {
    this.api.getEvents().subscribe(data => {
      this.events = data;
    });
    this.api.getSkippers().subscribe(data => {
      this.skippers = data;
    });
    this.router.params.subscribe(params => {
      if (params.id !== 'new') {
        this.loading = true;
        this.api.getShip(params.id).subscribe(data => {
          this.ship = data;
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

}
