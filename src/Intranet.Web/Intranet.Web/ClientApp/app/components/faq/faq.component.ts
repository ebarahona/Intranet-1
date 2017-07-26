import { Component, OnInit } from '@angular/core'
import { Faq } from '../../models'
import { FaqService } from '../../_services'

@Component({
  selector: 'faq',
  templateUrl: './faq.component.html',
  styleUrls: ['./faq.component.css']

})
export class FaqComponent implements OnInit {
  faqs: Faq[]

  constructor(
      private faqService: FaqService,
  ) { }

  ngOnInit() {
    this.updateData()
  }

  handleOnDelete(info: string) {
    this.updateData()
  }

  updateData() {
    this.faqService.getItems().subscribe(
        faqs => {
          console.log(faqs)
          this.faqs = faqs
        }
      )
  }
}
