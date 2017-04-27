import { Component, OnInit } from '@angular/core';
import {INewsItem} from '../../shared/interfaces';
import { DataService } from '../../shared/data_services/data.service';

@Component({
	selector: 'news',
	templateUrl: './news.component.html',
})
export class NewsComponent implements OnInit {
	newsitems: INewsItem[];
	selectedNewsitem: INewsItem;	

	constructor(private dataService: DataService) { }
	ngOnInit() {
		this.dataService.getNewsItems().subscribe((newsitems:INewsItem[]) => {
			this.newsitems = newsitems;
			console.log('Newsitems loaded')
		},
		error => {
			console.log('Failed to load newsitems '+error);
		});
	}

	onSelect(newsitem:INewsItem):void{
    	this.selectedNewsitem = newsitem;
    }

}

