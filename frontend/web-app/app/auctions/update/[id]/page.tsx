import React from "react";
import Heading from "../../componenets/Heading";
import AuctionForm from "../../AuctionForm";
import { getDetailedViewData } from "@/app/actions/auctionActions";

export default async function UpdateAuction({ params }: { params: { id: string } }) {
  const data = await getDetailedViewData(params.id);
  return (
    <div className="mx-auto max-w-[75%] shadow-lg p-10 bg-white rounded-lg">
      UpdateAuction {params.id}
      <Heading title="Update you Auction" subtitle="Please update the details of the car" />
      <AuctionForm auction={data} />
    </div>
  );
}
