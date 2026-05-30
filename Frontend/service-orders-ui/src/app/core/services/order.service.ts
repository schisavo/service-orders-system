import { HttpClient, HttpParams } from "@angular/common/http";
import { ServiceOrder } from "../models/service-order.model";
import { environment } from "../../../enviroments";
import { Injectable } from "@angular/core";

@Injectable({
  providedIn: 'root'
})
export class ServiceOrderService {

  private api =
    `${environment.apiUrl}/ServiceOrders`;

  constructor(
    private http: HttpClient
  ) {}

  getAll() {
    return this.http.get<ServiceOrder[]>(this.api);
  }

  getById(id:number) {
    return this.http.get<ServiceOrder>(`${this.api}/${id}`);
  }

  create(data:ServiceOrder) {
    return this.http.post(this.api, data);
  }

  update(id:number,data:ServiceOrder) {
    return this.http.put(`${this.api}/${id}`, data);
  }

  delete(id:number) {
    return this.http.delete(`${this.api}/${id}`);
  }

  filter(filters:any) {
    let params = new HttpParams();

    if(filters.estado)
      params = params.set('estado', filters.estado);

    if(filters.tecnico)
      params = params.set('tecnico', filters.tecnico);

    if(filters.cliente)
      params = params.set('cliente', filters.cliente);

    return this.http.get<ServiceOrder[]>(
      `${this.api}/filtro`,
      { params }
    );
  }
}
