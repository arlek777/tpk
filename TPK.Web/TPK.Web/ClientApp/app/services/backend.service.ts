import { Injectable } from "@angular/core";
import { Http } from "@angular/http";
import { ContentViewModel, ContentType, SiteViewModel } from "../models";
import 'rxjs/add/operator/toPromise';

@Injectable()
export class BackendService {
    constructor(private http: Http) { }

    getContent(id?: string): Promise<any> {
        var url;
        if (id) {
            url = `/api/content/get/${parseInt(id)}`;
        }
        else {
            url = "/api/content/get/";
        }

        return this.http.get(url).toPromise().then((result) => {
            return result.json();
        });
    }

    getSite(): Promise<SiteViewModel> {
        return this.http.get("/api/site/get").toPromise().then((result) => {
            return result.json();
        });
    }
}