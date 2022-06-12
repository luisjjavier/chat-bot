import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  constructor(private readonly router: Router) {
    this.loginForm = this.buildForm()
  }

  ngOnInit(): void {
  }

  private buildForm(): FormGroup {
     return new FormGroup({
      userName: new FormControl('', [Validators.required]),
      password: new FormControl('', [Validators.required])
    });
  }

  validateControl(controlName: string) {
    return this.loginForm.controls[controlName].invalid && this.loginForm.controls[controlName].touched
  }
  hasError(controlName: string, errorName: string) {
    return this.loginForm.controls[controlName].hasError(errorName)
  }
  goToRegister() {
    this.router.navigateByUrl('/register');
  }
  onLogin(login: any) {

  }
}
