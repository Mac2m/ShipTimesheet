import { TestBed } from '@angular/core/testing';

import { ShipTimesheetApiService } from './ship-timesheet-api.service';

describe('ShipTimesheetApiService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ShipTimesheetApiService = TestBed.get(ShipTimesheetApiService);
    expect(service).toBeTruthy();
  });
});
