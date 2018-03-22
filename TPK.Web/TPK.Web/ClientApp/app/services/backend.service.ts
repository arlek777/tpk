import { Injectable } from "@angular/core";
import { Http } from "@angular/http";
import { ContentViewModel, ContentType } from "../models";
import 'rxjs/add/operator/toPromise';

@Injectable()
export class BackendService {
    constructor(private http: Http) { }

    getRootCategories(): Promise<ContentViewModel[]> {
        return this.http.get("/api/content/getRootCategories").toPromise().then((result) => {
            return result.json();
        });
    }

    getContent(id: string): Promise<any> {
        return this.http.get("/api/content/getContent", { params: { id: id } }).toPromise().then((result) => {
            return result.json();
        });
    }

    getSite(): Promise<ContentViewModel[]> {
        return this.http.get("/api/site/get").toPromise().then((result) => {
            return result.json();
        });
    }
}