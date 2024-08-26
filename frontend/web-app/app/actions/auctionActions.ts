'use server'
import { Auction, PagedResult } from "@/types";
import Session from "../session/page";
import { auth } from "@/auth";

export async function getData(query:string) : Promise<PagedResult<Auction>>{
 
    const res = await fetch(`http://localhost:6001/search${query}`)

    if(!res.ok) throw new Error("Failed")
    console.log(res)
    return res.json();

}

export async function updateAuctionTest() {
    const data = {
        mileage: Math.floor(Math.random()*10000) +1
    }
    const session = await auth();

    const res = await fetch("http://localhost:6001/auctions/6a5011a1-fe1f-47df-9a32-b5346b289391" ,{
        method: 'PUT',
        headers:{
            'Content-Type' : 'application/json',
            'authorization': 'Bearer '+ session?.accessToken
        },
        body:JSON.stringify(data)
    });

    if(!res.ok) return {status : res.status,message:res.statusText}

    return res.json();
}