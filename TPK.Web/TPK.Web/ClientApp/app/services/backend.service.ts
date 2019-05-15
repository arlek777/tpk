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

    sendOrder(): Promise<boolean> {
        return this.http.get("/api/content/sendOrder").toPromise().then((result) => { return true }, (reason) => { return false; });
    }
}

@Injectable()
export class BasketService {
    private readonly basketKey = "basket";

    add(id: number) {
        const basket = sessionStorage.getItem(this.basketKey);
        if (basket) {
            let items: Array<number> = JSON.parse(sessionStorage[this.basketKey]).items;
            items.push(id);
            sessionStorage[this.basketKey] = JSON.stringify({ items: items });
        } else {
            sessionStorage[this.basketKey] = JSON.stringify({ items: [id] });
        }
    }

    remove(id: number) {
        let items: Array<number> = JSON.parse(sessionStorage[this.basketKey]).items;
        items = items.filter(i => i !== id);
        sessionStorage[this.basketKey] = JSON.stringify({ items: items });
    }

    getAll(): Array<number> {
        const basket = sessionStorage.getItem(this.basketKey);
        return basket ? JSON.parse(sessionStorage[this.basketKey]).items : [];
    }

    clear() {
        sessionStorage.removeItem(this.basketKey);
    }
}