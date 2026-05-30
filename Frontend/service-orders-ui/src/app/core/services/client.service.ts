import { HttpClient } from "@angular/common/http";
import { Client } from "../models/client.model";
import { environment } from "../../../enviroments";
import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root'
})
export class ClientService {

    private api = `${environment.apiUrl}/Clients`;

    constructor(
        private http: HttpClient
    ) {}

    getAll() {
        return this.http.get<Client[]>(
            this.api
        );
    }

    getById(id:number) {
        return this.http.get<Client>(
            `${this.api}/${id}`
        );
    }

    create(data:Client) {
        return this.http.post(
            this.api,
        data
        );
    }

    update(id:number,data:Client) {
        return this.http.put(
            `${this.api}/${id}`,
        data
        );
    }

    delete(id:number) {
        return this.http.delete(
            `${this.api}/${id}`
        );
    }
}
