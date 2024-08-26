import { getDetailedViewData } from "@/app/actions/auctionActions";
import React from "react";
import Heading from "../../componenets/Heading";
import CountDownTimer from "../../CountDownTimer";
import CarImages from "../../CarImages";
import DetailedSpecs from "./Detailedspaces";
import { getCurrentUser } from "@/app/actions/authActions";
import EditButton from "./EditButton";
import DeleteButton from "./DeleteButton";

export default async function Details({ params }: { params: { id: string } }) {
  const data = await getDetailedViewData(params.id);
  const user = await getCurrentUser()

  return (
    <div>
      <div className="flex justify-between">
        <div className="flex items-center gap-3"> 
        <Heading title={`${data.make} ${data.model}`} />
        {
          user?.username === data.seller && (
            <>
            <EditButton id = {data.id} />
            <DeleteButton  id = {data.id} />
            </>
          )

        }
        </div>

 
        <div className="flex gap-5">
          <h3 className="text-2xl font-semibold"> Time Remaining</h3>
          <CountDownTimer auctionEnd={data.auctionEnd} />
        </div>
      </div>
      <div className="grid grid-cols-2 gap-6 mt-3">
        <div className="w-full bg-gray-200 relative aspect-[4/3]">
        <CarImages imageUrl={data.imageUrl} />
        </div>

      <div className="border-2 rounded-lg p-2 bg-gray-100">
        <Heading title="Bids"/>
      </div>
      </div>
      <div className="mt-3 grid grid-cols-1 rounded-lg">
         <DetailedSpecs auction={data} />
      </div>

    </div>
  );
}
