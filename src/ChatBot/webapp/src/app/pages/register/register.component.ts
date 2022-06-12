import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserRegistration } from '../../models/user-registration';
import { AuthService } from '../../services/auth.service';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  registerForm: FormGroup;

  constructor(private readonly router: Router,
              private readonly authService: AuthService,
              private readonly notificationService: NotificationService) {
    this.registerForm = this.buildForm();
  }

  ngOnInit(): void {
  }

  private buildForm() {
    return new FormGroup({
      userName: new FormControl('', [Validators.required]),
      password: new FormControl('', [Validators.required]),
      confirm: new FormControl('', [Validators.required])
    });
  }

  validateControl(controlName: string) {
    return this.registerForm.controls[controlName].invalid && this.registerForm.controls[controlName].touched
  }

  hasError(controlName: string, errorName: string) {
    return this.registerForm.controls[controlName].hasError(errorName)
  }

  async goToLogin() {
    await this.router.navigateByUrl('/');
  }

   onRegister(newUserRegistration: any) {

    const user: UserRegistration = {
      userName: newUserRegistration.userName,
      password: newUserRegistration.password,
      confirmPassword: newUserRegistration.confirm
    };
     this.notificationService.showLoading();

    this.authService.registerUser(user).subscribe({
      next: async () => {
        await this.notificationService.showSuccessMessage('Registered successfully');
        await this.router.navigateByUrl('/');
      },
      error: async err => {
        console.log(err);
        await this.notificationService.showErrorMessage(err.error.error);
      }
    })
  }
}
