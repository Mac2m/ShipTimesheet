import { Component, OnInit } from '@angular/core';
import { ShipTimesheetApiService } from '../services/ship-timesheet-api.service';

@Component({
  selector: 'app-ships',
  templateUrl: './ships.component.html',
  styleUrls: ['./ships.component.css']
})
export class ShipsComponent implements OnInit {
  public ships = [];
  public loading = false;
  public totals = {};
  public pageSize = 5;
  public currPage = 1;

  constructor(private api: ShipTimesheetApiService) { }

  ngOnInit() {
    this.fetchData();
  }

  fetchData() {
    this.loading = true;
    this.api.getShips().subscribe(data => {
      this.ships = data;
      this.loading = false;
    });
  }

}
