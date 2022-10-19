import { Injectable } from '@angular/core';
import { connect, StringCodec } from "nats.ws";
import { CommunicationService } from './CommunicationService';

@Injectable({
  providedIn: 'root'
})
export class ReportRpcService {

  public connection: any;
  public natsSub: any;

  constructor(private communicationService: CommunicationService) { }

  async listenNats() {
		const sc = StringCodec();
		for await (const m of this.natsSub) {
			let message = sc.decode(m.data);

			console.log(`Topic: '${m.subject}' - Message: '${message}'`);
      this.communicationService.pub<string>("rpc",{content:message,sender:"asd"});
		}
		console.log("Subscription closed");
	}


  public connectToNats() {
		connect({ servers: 'ws://localhost:5222' })
			.then(async (conn: any) => {
				this.connection = conn;
				console.log(`Connected to ${this.connection.getServer()}`);
				this.natsSub = this.connection.subscribe("PocReport");
				this.listenNats();
			});
	}
}
