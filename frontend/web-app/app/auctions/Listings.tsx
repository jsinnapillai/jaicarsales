"use client";
import React, { useEffect, useState } from "react";
import AuctionCard from "./AuctionCard";
import { Auction, PagedResult } from "@/types";
import AppPagination from "./componenets/AppPagination";
import { getData } from "../actions/auctionActions";
import Filter from "./Filter";
import { useParamsStore } from "@/hooks/useParamStore";
import { useShallow } from "zustand/react/shallow";
import qs from "query-string";
import EmptyFilter from "./componenets/EmptyFilter";

export default function Listings() {
  const [data, setData] = useState<PagedResult<Auction>>();
  const params = useParamsStore(
    useShallow((state) => ({
      pageNumber: state.pageNumber,
      pageSize: state.pageSize,
      searchTerm: state.searchTerm,
      orderBy: state.orderBy,
      filterBy: state.filterBy,
      winner: state.winner,
      seller: state.seller
    }))
  );

  const setParams = useParamsStore((state) => state.setParams);
  const url = qs.stringifyUrl({ url: "", query: params });

  function setPageNumber(pageNumber: number) {
    setParams({ pageNumber });
  }
  // const [auctions,setAucations] = useState<Auction[]>();
  // const [pageCount,setPageCount] = useState<number>(0);
  // const [pageNumber,setPageNumber] = useState<number>(1);
  // const [pageSize,setPageSize] = useState<number>(4);

  useEffect(() => {
    getData(url).then((data) => {
      setData(data);
    });
  }, [url]);

  if (!data) {
    return <h1>Loading...</h1>;
  }

 

  return (
    <>
      <Filter />
      {data.totalCount === 0 ? (
        <EmptyFilter showReset />
      ) : (
        <>
          <div className="grid grid-cols-4 gap-6">
            {data &&
              data.result.map((auction: Auction, index: number) => (
                <AuctionCard auction={auction} key={index} />
              ))}
          </div>
          <div className="flex justify-center">
            <AppPagination
              pageChanged={setPageNumber}
              currentPage={params.pageNumber}
              pageCount={data.pageCount}
            />
          </div>
        </>
      )}
    </>
  );
}
