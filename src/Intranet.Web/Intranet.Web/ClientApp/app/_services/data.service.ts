﻿import { Injectable } from '@angular/core'
import { Response, Headers, RequestOptions } from '@angular/http'

// Grab everything with import 'rxjs/Rx'
import { Observable } from 'rxjs/Observable'
import { Observer } from 'rxjs/Observer'
import 'rxjs/add/observable/throw'
import 'rxjs/add/operator/map'
import 'rxjs/add/operator/catch'
import 'rxjs/add/operator/toPromise'

import { AuthenticationService, ConfigService, SecureHttpService } from './'

import { News, Checklist, Profile } from '../models'

@Injectable()
export class DataService {
    constructor(
        private http: SecureHttpService,
    ) { }

    // PROFILE SERVICE ************************************************  /

     // get all Profiles
    getAllProfiles(): Observable<Profile[]> {
        return this.http.get('profile')
            .map((res: Response) => {
                return res.json()
            })
            .catch(this.handleError)
    }

    // get a specific profile by Id
    getProfile(id: number): Observable<Profile> {
        return this.http.get('profile/' + id)
            .map((res: Response) => {
                return res.json()
        })
        .catch(this.handleError)
    }

     // create new Profile
    //  createProfile(firstName: string, lastName: string, description: string, email: string, phoneNumber: number, mobile: number, streetAdress: string, postalCode: number, city: string): Promise<Profile> {
    //    // TODO: Refactor!!!
    //  const body = JSON.stringify({firstName: firstName, lastName: lastName, description: description, email: email, phoneNumber: phoneNumber, mobile: mobile, streetAdress: streetAdress, postalCode: postalCode, city: city})
    //    return this.http.post('profile/', body, { headers: this.headers })
    //            .toPromise()
    //            .then(res => res.json().data as Profile)
    //            .catch(this.handleError)
    //  }

    // update a Profile
    updateProfile(profile: Profile): Observable<void> {
        const headers = new Headers()
        headers.append('Content-Type', 'application/json')
        headers.append('Access-Control-Allow-Origin', '*')
        return this.http.put('profile/' + profile.id, JSON.stringify(profile), {
            headers: headers
        })
            .map((res: Response) => {
                return
            })
            .catch(this.handleError)
    }

    // delete Profile
    deleteProfile(id: number): Observable<void> {
        return this.http.delete('profile/' + id)
            .map((res: Response) => {
                return
            })
            .catch(this.handleError)
    }

    // CHECKLIST SERVICE ************************************************  /

    // get profiles checklist
    getProfileChecklist(id: number): Observable<Checklist[]> {
        return this.http.get('profile/' + id + '/checklist')
            .map((res: Response) => {
                return res.json()
            })
            .catch(this.handleError)
    }

    // update checklist
    updateProfileChecklist(profileId: number, todo: Checklist): Observable<void> {
        const headers = new Headers()
        headers.append('Content-Type', 'application/json')
        headers.append('Access-Control-Allow-Origin', '*')
        return this.http.put('profile/' + profileId + '/checklist/' + todo.toDoId, JSON.stringify(todo), {
            headers: headers
        })
            .map((res: Response) => {
                return
            })
            .catch(this.handleError)
    }

    // async uploadFile(file): Promise<string> {
    //     if (typeof file !== 'undefined') {

    //         const headers = new Headers(
    //         {
    //             'Accept': 'application/json'
    //         })

    //         const formData: FormData = new FormData()

    //         formData.append(file.name, file)
    //         // headers.append('Content-Type', 'multipart/form-data')
    //         // DON'T SET THE Content-Type to multipart/form-data
    //         // You'll get the Missing content-type boundary error
    //         const options = new RequestOptions({ headers: headers })

    //         const response = await this.http.post('upload', formData, options, true).toPromise()

    //         return response.json().fileName
    //     }

    //     return null
    // }

    private handleError (error: Response | any) {
        const applicationError = error.headers.get('constlication-Error')
        const serverError = error.json()
        let modelStateErrors: string = ''

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
