import { Injectable } from '@angular/core'
import { Response, Headers, RequestOptions } from '@angular/http'

// Grab everything with import 'rxjs/Rx'
import { Observable } from 'rxjs/Observable'
import { Observer } from 'rxjs/Observer'
import 'rxjs/add/observable/throw'
import 'rxjs/add/operator/map'
import 'rxjs/add/operator/catch'
import 'rxjs/add/operator/toPromise'

import { SecureHttpService } from './'
import { RestService } from './rest.service'

import { Faq } from '../models'

@Injectable()
export class FaqService extends RestService<Faq> {
  constructor(
    http: SecureHttpService,
  ) {
    super(http, 'faq')
  }
}
