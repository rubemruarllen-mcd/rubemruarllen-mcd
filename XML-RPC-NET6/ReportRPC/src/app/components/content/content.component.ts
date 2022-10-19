import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { CommunicationService } from 'src/app/services/CommunicationService';
import {MatFormFieldModule} from '@angular/material/form-field';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-content',
  templateUrl: './content.component.html',
  styleUrls: ['./content.component.scss']
})
export class ContentComponent implements OnInit {

  public rpc:string = "";
  private rpcM: Observable<any> = this.communicationService.sub<string>("rpc","")
  public angForm: FormGroup;
  private dateS: Date = new Date();
  public date: any
  public hasRes: boolean = false;
  public requestRunning = false;
  public moreContent = false;
  public gotFullReport =  false;
  public watchB: boolean = false;
  public updated = false;



  public selected: number = 1;


  constructor(private communicationService: CommunicationService,private fb: FormBuilder, private datePipe: DatePipe, private httpClient: HttpClient) { 

    
    this.date = this.datePipe.transform(this.dateS, 'yyyy/MM/dd');
    console.log(this.date)

    
    this.rpcM.subscribe(re=>{
      if(this.watchB){
        let result:string = re;
        if(result.includes("UPDATED-A0")){
          this.updated=true;
          this.rpc = "waiting new events";
        }
        else if(this.updated){
          this.rpc = "--------------------NEW EVENT ------------------------"+'\n'+re+'\n'+"-----------------Waiting new events------------------";
        }
        else{
          this.rpc += re;
        }
      }
      else{this.rpc += re;}
      
      this.hasRes= true;
      this.moreContent = true;

      

      setTimeout(()=>{
        this.moreContent = false;
      },1000)
    });
    //this.communicationService.pub<string>("rpc",{content:"asdasd",sender:"as"})
    this.angForm = this.fb.group({
      businessDay: [],
      checkPoint: [],
      cypherKey: [],
      datatype: [],
      piiMaskIdType: [],
      piiMaskType: [],
      posList: [],
      recordId: [],
      maxEvents: []
   });
   this.angForm.get("checkPoint")?.setValue("0");
   this.angForm.get("cypherKey")?.setValue("");
   this.angForm.get("datatype")?.setValue("STLD");
   this.angForm.get("piiMaskIdType")?.setValue("0");
   this.angForm.get("piiMaskType")?.setValue("0");
   this.angForm.get("posList")?.setValue("");
   this.angForm.get("recordId")?.setValue("");
   this.angForm.get("maxEvents")?.setValue("10");
   this.angForm.get("businessDay")?.setValue(this.date);

   this
  }

	public selectedV = 'full';
	onSelected(value:string): void {
		this.selectedV = value;
    this.hasRes = false;
    this.requestRunning = false;
    this.gotFullReport = false;
    this.watchB=false;
    this.updated = false;

	}



  request(){
    this.rpc = "";
    let payload = {
      dataType: this.angForm.get("datatype")?.value,
      posList: this.angForm.get("posList")?.value,
      checkPoint: this.angForm.get("checkPoint")?.value,
      piiMaskType: this.angForm.get("piiMaskType")?.value,
      cypherKey: this.angForm.get("cypherKey")?.value,
      piiMaskIdType: this.angForm.get("piiMaskIdType")?.value,
      recordId: this.angForm.get("recordId")?.value,
      maxEvents: this.angForm.get("maxEvents")?.value,
    }
    this.requestRunning =true;
    console.log(payload)
    let headers = new HttpHeaders();
    headers = headers.set('businessDay',this.date);
    let url = "http://localhost:1998/api/v1/StldReport"
    this.httpClient.post<string>(url,payload,{headers, responseType: 'text' as 'json' }).subscribe(result=>{
      this.rpc=result;
      this.requestRunning =false;
      this.gotFullReport =true;
    });

    
  }


  watch(){
    this.rpc = "";
    this.watchB = true;
    console.log("OK");
    
    this.requestRunning =true;

    let headers = new HttpHeaders();
    headers = headers.set('businessDay',this.date);
    let url = "http://localhost:1998/api/v1/StldReportReal"
    this.httpClient.get<string>(url).subscribe(result=>{

    });

    
  }

  ngOnInit(): void {
  }

}
