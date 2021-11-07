import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-errors',
  templateUrl: './errors.component.html',
  styleUrls: ['./errors.component.css'],
})
export class ErrorsComponent implements OnInit {
  private baseUrl = environment.baseUrl;
  validationErrors: string[] = [];
  constructor(private http: HttpClient) {}

  ngOnInit(): void {}

  getNotFound() {
    return this.http.get(this.baseUrl + '/buggy/not-found').subscribe(
      (response) => {
        //console.log(response);
      },
      (error) => {
        //console.log(error);
      }
    );
  }
  getServerError() {
    return this.http.get(this.baseUrl + '/buggy/server-error').subscribe(
      (response) => {
        //console.log(response);
      },
      (error) => {
        //console.log(error);
      }
    );
  }

  getBadRequest() {
    return this.http.get(this.baseUrl + '/buggy/bad-request').subscribe(
      (response) => {
        //console.log(response);
      },
      (error) => {
        //console.log(error);
      }
    );
  }

  getValidationError() {
    return this.http.get(this.baseUrl + '/buggy/bad-request/one').subscribe(
      (response) => {
        //console.log(response);
      },
      (error) => {
        //console.log(error);
      }
    );
  }

  getValidationErrorRegister() {
    return this.http.post(`${this.baseUrl}/account/register`, {}).subscribe(
      (response) => {
        // console.log(response);
      },
      (error) => {
        this.validationErrors = error.error.errors;
      }
    );
  }
}
