import { Component, OnInit, ViewChild } from '@angular/core';
import { ShipTimesheetApiService } from '../services/ship-timesheet-api.service';
import { FullCalendarComponent } from '@fullcalendar/angular';
import { EventInput } from '@fullcalendar/core';
import {NgbModal, ModalDismissReasons} from '@ng-bootstrap/ng-bootstrap';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGrigPlugin from '@fullcalendar/timegrid';
import interactionPlugin from '@fullcalendar/interaction';
import bootstrapPlugin from '@fullcalendar/bootstrap';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  @ViewChild('calendar') calendarComponent: FullCalendarComponent;

  closeResult = '';
  calendarVisible = true;
  calendarPlugins = [
    dayGridPlugin,
    timeGrigPlugin,
    interactionPlugin,
    bootstrapPlugin,
  ];
  calendarWeekends = true;
  calendarEvents: EventInput[] = [];
  theme: string = 'bootstrap';
  fontAwesome = false;
  timeFormat = {
    hour: '2-digit',
    minute: '2-digit',
    hour12: false,
  };
  slotLabelFormat= {
    hour: 'numeric',
    minute: '2-digit',
    omitZeroMinute: true,
    hour12: false
  };
  locale = 'pl';

  public events = [];
  public event: { eventTime: any; shipId: any; eventType: any; } = { eventTime: '', shipId: '', eventType: null };
  public ships: any[];

  public dateFilter: {year: any, month: any, day: any};

  constructor(
    private api: ShipTimesheetApiService,
    private modalService: NgbModal
  ) {}

  ngOnInit() {
    this.api.getEvents().subscribe((data) => {
      this.events = data;
      this.calendarEvents = this.events.map((event: any) => {
        return {
          title: event.ship.name,
          date: event.eventTime,
          type: event.eventType,
          color: event.eventType === 1 ? 'green' : 'red',
        };
      });
    });
    this.api.getShips().subscribe(data => {
      this.ships = data;
    });
  }

  gotoDate() {
    let calendarApi = this.calendarComponent.getApi();
    calendarApi.gotoDate(new Date(this.dateFilter.year, this.dateFilter.month - 1, this.dateFilter.day).toISOString());
  }

  clear() {
    this.dateFilter = undefined;
    let calendarApi = this.calendarComponent.getApi();
    calendarApi.gotoDate(new Date().toISOString());
  }

  handleDateClick(content, arg) {
    this.event.eventTime = arg.date;
    this.modalService.open(content, {ariaLabelledBy: 'modal-basic-title'});
  }

  saveNewEvent(){
    this.api.addEvent(this.event).subscribe(data => {
      this.ngOnInit();
      this.modalService.dismissAll();
    });
  }
}

