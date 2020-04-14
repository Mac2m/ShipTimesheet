import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ShipsEditComponent } from './ships-edit.component';

describe('ShipsEditComponent', () => {
  let component: ShipsEditComponent;
  let fixture: ComponentFixture<ShipsEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ShipsEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShipsEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
