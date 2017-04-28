import { Injectable } from '@angular/core'
import { Http, Response, Headers } from '@angular/http'

// Grab everything with import 'rxjs/Rx'
import { Observable } from 'rxjs/Observable'
import {Observer} from 'rxjs/Observer'
import 'rxjs/add/operator/map'
import 'rxjs/add/operator/catch'
import 'rxjs/add/operator/toPromise'

import { INewsItem, IChecklist } from '../interfaces'
import { ConfigService } from '../api_settings/config.service'
import { TokenService } from '../data_services/jwt-token.service'

@Injectable()
export class DataService {

    private headers = new Headers({'Content-Type': 'application/json'})

    _baseUrl: string = ''
    _mockUrl: string = ''

    constructor(private http: Http,
                private configService: ConfigService) {
                this._mockUrl = configService.getMockURI()
                this._baseUrl = configService.getApiURI()
    }

// NEWS SERVICES ************************************************************  /

    // get all News
    getNewsItems(): Observable<INewsItem[]> {
        return this.http.get(this._baseUrl)
            .map((res: Response) => {
                return res.json()
            })
            .catch(this.handleError)
    }

    // get a specific news by Id
    getNewsItem(id: number): Observable<INewsItem> {
        return this.http.get(this._baseUrl + id)
            .map((res: Response) => {
                return res.json()
            })
            .catch(this.handleError)
    }

    // create a new NewsItem
    createNewsItem(title: string, date: Date, text: string, author: string): Observable< INewsItem> {
         const body = JSON.stringify({title: title, date: date,  text: text, author: author})

          return this.http.post(this._baseUrl, body, { headers: this.headers })
                        .map(res => res.json().data as INewsItem)
                        .catch(this.handleError)
    }

// NEW AT CERTAINCY SERVICE ************************************************  /

    // get checklista
    getNewAtCert(): Observable<IChecklist[]> {
        return this.http.get(this._mockUrl)
            .map((res: Response) => {
                return res.json()
        })
        .catch(this.handleError)
    }

    private handleError (error: Response | any) {
        const applicationError = error.headers.get('constlication-Error')
        const serverError = error.json()
        const modelStateErrors: string = ''

        if (!serverError.type) {
            console.log(serverError)
            for (const key in serverError) {
                if (serverError[key])
                    modelStateErrors += serverError[key] + '\n'
            }
        }

        modelStateErrors = modelStateErrors = '' ? null : modelStateErrors

        return Observable.throw(applicationError || modelStateErrors || 'Server error')
    }
}