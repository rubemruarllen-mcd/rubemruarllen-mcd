import { TestBed } from '@angular/core/testing';

import { ReportRpcService } from './report-rpc.service';

describe('ReportRpcService', () => {
  let service: ReportRpcService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ReportRpcService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
