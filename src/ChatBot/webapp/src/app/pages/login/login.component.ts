import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginRequest } from '../../models/login-request';
import { AuthService } from '../../services/auth.service';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  constructor(private readonly router: Router,
  private readonly notificationService: NotificationService,
  private readonly authService: AuthService) {
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
      this.notificationService.showLoading();
      const loginRequest: LoginRequest = {
        userName: login.userName,
        password: login.password
      }
      this.authService.doLogin( loginRequest).subscribe({
        next: (response: any) => {
          localStorage.setItem("token", response.token);
          this.router.navigateByUrl('/chatroom');
          this.notificationService.closeLoading();
        },
        error: async err => {
          await this.notificationService.showErrorMessage(err.error.error);
        }
      })
  }
}
