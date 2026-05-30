import { Component } from '@angular/core';
import { FormBuilder, Validators, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule],
  templateUrl: './login.component.html'
})
export class LoginComponent {
  form: FormGroup;

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private router: Router
  ) {
    this.form = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

    submit() {
        if (this.form.invalid) return;

        const data = this.form.getRawValue() as { username: string; password: string };

            this.auth.login(data).subscribe({
                next: (res) => {
                this.auth.saveToken(res.token);
                this.router.navigate(['/dashboard']);
                },
                error: () => alert('Credenciales inválidas')
            });
    }

}
