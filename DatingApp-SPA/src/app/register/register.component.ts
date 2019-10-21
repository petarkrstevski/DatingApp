import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  @Output() cancelRegister = new EventEmitter();
  @Input() valuesFromHome : any;
  model : any = {};
  constructor(private auth:AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
  }


  cancel(){

    this.cancelRegister.emit(false);
    // this.alertify.message("Canceled!");
  }

  register(){
    console.log(this.model);

    this.auth.register(this.model).subscribe(next => {
      this.alertify.success('registration succesfully!');
    },error=> {
      this.alertify.error(error);
    });
  }

}
