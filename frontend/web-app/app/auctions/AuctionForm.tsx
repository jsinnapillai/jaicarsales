"use client";
import { Button } from "flowbite-react";
import React, { useEffect } from "react";
import { FieldValues, useForm } from "react-hook-form";
import Input from "./componenets/Input";
import DateInput from "./componenets/DateInput";
import { createAuction, updateAuctions } from "../actions/auctionActions";
import { usePathname, useRouter } from "next/navigation";
import toast from "react-hot-toast";
import { Auction } from "@/types";


type Props = {
  auction?: Auction
}

export default function AuctionForm({auction}: Props) {
    const router = useRouter();
    const pathname = usePathname()

  const {
    control,
    register,
    handleSubmit,
    setFocus,
    reset,
    formState: { isLoading, isSubmitting, isValid, isDirty, errors },
  } = useForm({ mode: "onTouched" });

  const onSubmit = async (data: FieldValues) => {
    try {
      let id = ""
      let res;
      if(pathname === "/auctions/create")
      {
        res = await createAuction(data);
      id = res.id
    }else
    {
        res = await updateAuctions(data,auction!.id);
      id = auction!.id
    }
      if (res.error) {
        throw  res.error ;
      }
      // Sucessful then redirecting to the details page
      router.push(`/auctions/details/${id}`);
    } catch (error:any) {
 
      toast.error(error.status + ' ' + error.message)
    }
  };
  useEffect(() => {
    if(auction)
    {
      const {make,model,color,mileage,year} = auction;
      reset({make,model,color,mileage,year})
    }
    setFocus("make");
  }, [setFocus]);

  return (
    <>
      <form className="flex flex-col mt-3" onSubmit={handleSubmit(onSubmit)}>
        <Input
          label="Make"
          name="make"
          type="text"
          control={control}
          rules={{ required: "Make is required" }}
        />
        <Input
          label="Model"
          name="model"
          type="text"
          control={control}
          rules={{ required: "Model is required" }}
        />
                <Input
          label="Color"
          name="color"
          type="text"
          control={control}
          rules={{ required: "color is required" }}
        />
        <div className="grid grid-cols-2 gap-3">
          <Input
            label="Year"
            name="year"
            control={control}
            type="number"
            rules={{ required: "Year is required" }}
          />
          <Input
            label="Mileage"
            name="mileage"
            control={control}
            type="number"
            rules={{ required: "Mielage is required" }}
          />
        </div>
        { pathname === "/auctions/create" && 
      <>

        <Input
          label="Image Url"
          name="imageurl"
          type="text"
          control={control}
          rules={{ required: "Image Url is required" }}
        />

        <div className="grid grid-cols-2 gap-3">
          <Input
            label="Reserve Price (Enter 0 if no reserve)"
            name="reservePrice"
            control={control}
            type="number"
            rules={{ required: "Reserve Price is required" }}
          />
          <DateInput
            label="Auction End (Date/time)"
            name="auctionend"
            type="date"
            dateFormat="dd MMM yyy h:mm a"
            showTimeSelect
            control={control}
            rules={{ required: "Auction end date is required" }}
          />
        </div>
        </>
      }

        <div className="flex justify-between">
          <Button outline color="gray">
            Cancel
          </Button>
          <Button
            isProcessing={isSubmitting}
            disabled={!isValid}
            outline
            color="success"
            type="submit"
          >
            submit
          </Button>
        </div>
      </form>
    </>
  );
}
