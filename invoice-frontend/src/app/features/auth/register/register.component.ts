import { Component, inject } from '@angular/core';
import {
  FormBuilder,
  Validators,
  ReactiveFormsModule,
  AbstractControl,
  ValidationErrors
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '../../../core/services/auth.service';
import { NotificationService } from '../../../core/services/notification.service';

function passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
  const password = control.get('password');
  const confirm  = control.get('confirmPassword');

  if (!password || !confirm) return null;
  if (password.value !== confirm.value) {
    confirm.setErrors({ passwordMismatch: true });
    return { passwordMismatch: true };
  }

  // Clear the error if they now match
  const errors = { ...confirm.errors };
  delete errors['passwordMismatch'];
  confirm.setErrors(Object.keys(errors).length ? errors : null);

  return null;
}

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatIconModule
  ],
  templateUrl: './register.component.html'
})
export class RegisterComponent {
  private readonly fb           = inject(FormBuilder);
  private readonly auth         = inject(AuthService);
  private readonly router       = inject(Router);
  private readonly notifications = inject(NotificationService);

  loading      = false;
  errorMessage: string | null = null;
  hidePassword        = true;
  hideConfirmPassword = true;

  form = this.fb.group(
    {
      email:           ['', [Validators.required, Validators.email]],
      password:        ['', [Validators.required, Validators.minLength(8),
                             Validators.pattern(/(?=.*[A-Z])(?=.*[0-9])/)]],
      confirmPassword: ['', Validators.required]
    },
    { validators: passwordMatchValidator }
  );

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.loading      = true;
    this.errorMessage = null;

    const { email, password } = this.form.getRawValue();

    this.auth.register({ email: email!, password: password! }).subscribe({
      next: () => {
        this.notifications.success('Account created. Please sign in.');
        this.router.navigate(['/login']);
      },
      error: err => {
        this.errorMessage =
          err.error?.errors?.[0] ??
          err.error?.error ??
          'Registration failed. Please try again.';
        this.loading = false;
      }
    });
  }
}