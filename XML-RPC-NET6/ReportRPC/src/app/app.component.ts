import { Component } from '@angular/core';
import { ReportRpcService } from './services/report-rpc.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'ReportRPC';

  constructor(private rp: ReportRpcService){
    rp.connectToNats();
  }
}
