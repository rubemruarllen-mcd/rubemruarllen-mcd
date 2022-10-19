import { Injectable } from '@angular/core';
import { fromEvent, Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';

/**
 * Used to encapsulate the payload of a message, with the sender information.
 */
export interface DataHolder<T> {
  /**
   * The name of the entity that has sent the message.
   */
  sender: string,

  /**
   * Payload of the message
   */
  content: T
}


@Injectable({
  providedIn: 'root'
})
export class CommunicationService {

  constructor() { }

  /**
   * Send a message
   * @param subject The topic to publish a message
   * @param content Payload. If provided with sender, it will be taken in consideration
   * when subscribing to events, in order to avoid self spamming messages.
   */
  pub<T>(subject: string, content: DataHolder<T>): void {
    const evt = new CustomEvent(subject, {
      detail: content
    });

    window.dispatchEvent(evt);
  }


  /**
   * Subscribe for receive messages
   * @param subject The topic to listen for messages
   * @param listener Name of the listener to avoid receiving messages that itself posted.
   * @returns An observable that respond to events published on the subject
   */
  sub<T>(subject: string, listener?: string): Observable<T> {
    const obs = fromEvent(window, subject);
    return obs.pipe(
      // Filter to avoid self messaging
      filter((data: Event) => {
        const customEvt = data as CustomEvent<DataHolder<T>>;
        const dataHolder = customEvt?.detail;
        return listener === undefined || listener != dataHolder.sender;
      }),
      // Map from Event to <T>
      map((data: Event) => {
        const customEvt = data as CustomEvent<DataHolder<T>>;
        const dataHolder = customEvt?.detail;
        return dataHolder.content;
      }));
  }
}
