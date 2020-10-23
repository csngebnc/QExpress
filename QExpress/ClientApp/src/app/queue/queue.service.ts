import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class QueueService {

    active_queue = [];

  constructor() {}

  addToActive(queue){
      this.active_queue.push(queue);
  }

  getActiveQueue(){
      return this.active_queue;
  }

  deleteQueue(id){
    var index = this.active_queue.findIndex(x => x._id == id)
    if (index > -1) {
        this.active_queue.splice(index, 1);
    }
  }

}