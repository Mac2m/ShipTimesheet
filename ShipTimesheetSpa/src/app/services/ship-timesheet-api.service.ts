import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ShipTimesheetApiService {
  private baseUrl = 'https://localhost:44378/api/v1';

  constructor(private http: HttpClient) { }

  getEvents() {
    return this.http.get<any[]>(`${this.baseUrl}/events`).pipe(map((data: any) => data.value ),
            catchError(error => throwError(error)));
  }

  getEventsPaged(currPage, pageSize) {
    return this.http.get<any[]>(`${this.baseUrl}/events?_page=${currPage}&_limit=${pageSize}`);
  }

  getEvent(id) {
    return this.http.get<any>(`${this.baseUrl}/events/${id}`);
  }

  addEvent(event: any) {
    return this.http.post(`${this.baseUrl}/events`, event);
  }

  updateEvent(event: any) {
    return this.http.put(`${this.baseUrl}/events/${event.eventId}`, event);
  }

  saveEvent(event: any) {
    if (event.eventId) {
      return this.updateEvent(event);
    } else {
      return this.addEvent(event);
    }
  }

  deleteEvent(eventId) {
    return this.http.delete(`${this.baseUrl}/events/${eventId}`);
  }

  // ships

  getShips() {
    return this.http.get<any[]>(`${this.baseUrl}/ships`).pipe(map((data: any) => data.value ),
            catchError(error => throwError(error)));
  }

  getShipsPaged(currPage, pageSize) {
    return this.http.get<any[]>(`${this.baseUrl}/ships?_page=${currPage}&_limit=${pageSize}`);
  }

  getShip(id) {
    return this.http.get<any>(`${this.baseUrl}/ships/${id}`);
  }

  addShip(ship: any) {
    return this.http.post(`${this.baseUrl}/ships`, ship);
  }

  updateShip(ship: any) {
    return this.http.put(`${this.baseUrl}/ships/${ship.shipId}`, ship);
  }

  saveShip(ship: any) {
    if (ship.shipId) {
      return this.updateShip(ship);
    } else {
      return this.addShip(ship);
    }
  }

  // skippers

  getSkippers() {
    return this.http.get<any[]>(`${this.baseUrl}/skippers`).pipe(map((data: any) => data.value ),
            catchError(error => throwError(error)));
  }

  getSkippersPaged(currPage, pageSize) {
    return this.http.get<any[]>(`${this.baseUrl}/skippers?_page=${currPage}&_limit=${pageSize}`);
  }

  getSkipper(id) {
    return this.http.get<any>(`${this.baseUrl}/skippers/${id}`);
  }

  addSkipper(skipper: any) {
    return this.http.post(`${this.baseUrl}/skipper`, skipper);
  }

  updateSkipper(skipper: any) {
    return this.http.put(`${this.baseUrl}/skippers/${skipper.skipperId}`, skipper);
  }

  saveSkipper(skipper: any) {
    if (skipper.skipperId) {
      return this.updateSkipper(skipper);
    } else {
      return this.addSkipper(skipper);
    }
  }

}
