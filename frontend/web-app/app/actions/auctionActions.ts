"use server";
import { Auction, PagedResult } from "@/types";
import Session from "../session/page";
import { auth } from "@/auth";
import { fetchWrapper } from "../lib/fetchWrapper";
import { FieldValues } from "react-hook-form";
import { revalidatePath } from "next/cache";

export async function getData(query: string): Promise<PagedResult<Auction>> {
  console.log(query);
  return await fetchWrapper.get(`search${query}`);
}

export async function updateAuctionTest() {
  const data = {
    mileage: Math.floor(Math.random() * 10000) + 1,
  };
  return await fetchWrapper.put(
    "auctions/6a5011a1-fe1f-47df-9a32-b5346b289391",
    data
  );

  // const session = await auth();

  // const res = await fetch("http://localhost:6001/auctions/6a5011a1-fe1f-47df-9a32-b5346b289391" ,{
  //     method: 'PUT',
  //     headers:{
  //         'Content-Type' : 'application/json',
  //         'authorization': 'Bearer '+ session?.accessToken
  //     },
  //     body:JSON.stringify(data)
  // });

  // if(!res.ok) return {status : res.status,message:res.statusText}

  // return res.json();
}

export async function createAuction(data: FieldValues) {
  return await fetchWrapper.post("auctions", data);
}

export async function getDetailedViewData(id: string) :Promise<Auction>  {
    return await fetchWrapper.get(`auctions/${id}`);
  }
  

  export async function updateAuctions(data: FieldValues,id:string) {
    const res =  await fetchWrapper.put(`auctions/${id}`, data);
    revalidatePath(`/auctions/${res.id}`);
    return res;
  } 

  export async function deleteAuction(id: string)    {
    return await fetchWrapper.del(`auctions/${id}`);
  }
  