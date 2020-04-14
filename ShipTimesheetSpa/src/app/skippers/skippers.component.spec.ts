import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SkippersComponent } from './skippers.component';

describe('SkippersComponent', () => {
  let component: SkippersComponent;
  let fixture: ComponentFixture<SkippersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SkippersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SkippersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
