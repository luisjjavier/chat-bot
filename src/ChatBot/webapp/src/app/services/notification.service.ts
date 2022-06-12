import { Injectable } from '@angular/core';
import Swal from 'sweetalert2';
@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  constructor() { }

  async showWarningMessage(message: string) {
    await Swal.fire({
      icon: 'warning',
      text: message
    });
  }

  async showErrorMessage(message: string) {
    await Swal.fire({
      icon: 'error',
      text: message
    });
  }

  async showErrorMessages(errors: string[], title: string) {
    const notificationBody = this.getErrorsAsHTMLList(errors);

    await Swal.fire({
      title: title,
      icon: 'error',
      html: notificationBody
    });
  }

  private getErrorsAsHTMLList(errors: string[]) {
    let response = '<ul>';
    errors.forEach(error => { response += `<li class="text-danger">${error}</li>`});
    return response + '</ul>';
  }

  async showSuccessMessage(message: string) {
   await Swal.fire({
      icon: 'success',
      text: message
    });
  }

  async showInfoMessage(message: string) {
    await Swal.fire({
      icon: 'info',
      text: message
    });
  }

   showLoading() {
     Swal.fire({
      allowOutsideClick: false,
      showConfirmButton: false,
      didOpen(popup: HTMLElement) {
        Swal.showLoading()
      }
    });
  }

  closeLoading() {
    Swal.close();
  }
}
